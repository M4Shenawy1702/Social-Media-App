import {
  Component,
  OnInit,
  OnDestroy,
  ViewChild,
  ElementRef,
  AfterViewChecked
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ChatSignalrService } from '../Services/chat-signal-r.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ChatMessage } from '../shared/Contracts/ChatMessage';
import { UserProfile } from '../shared/Contracts/UserProfile';
import { UserService } from '../Services/user.service';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'; 

interface ChatMessageWithUI extends ChatMessage {
  showMenu?: boolean;
}

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit, OnDestroy, AfterViewChecked {
  messages: ChatMessageWithUI[] = [];
  newMessage = '';
  receiverId = '';
  currentUserId = '';
  user: UserProfile | null = null;
  baseUrl = environment.baseUrl;
  editMessage: ChatMessageWithUI | null = null;

  @ViewChild('messagesContainer') private messagesContainer!: ElementRef;

  constructor(
    private chatService: ChatSignalrService,
    private route: ActivatedRoute,
    private userService: UserService
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.receiverId = params.get('id') || '';
      const currentUser = localStorage.getItem('user');

      if (!currentUser) {
        console.error('User not logged in or token missing');
        return;
      }

      const userObj = JSON.parse(currentUser);
      this.currentUserId = userObj.userId;
      const token = userObj.token;

      this.chatService.startConnection(token);

      this.userService.getUserById(this.receiverId).subscribe({
        next: (profile) => (this.user = profile),
        error: (err) => console.error('Failed to load user profile:', err)
      });

      this.chatService.getMessages(this.receiverId, (msgs) => {
        this.messages = msgs.map(msg => ({ ...msg }));
        this.scrollToBottom();
      });

      this.chatService.onMessageReceived((senderId, message) => {
        this.messages.push({
          senderId,
          content: message,
          timestamp: new Date().toISOString()
        });
        this.scrollToBottom();
      });
    })
  }

  ngAfterViewChecked(): void {
    this.scrollToBottom();
  }

  sendMessage(): void {
    const trimmedMessage = this.newMessage.trim();
    if (!trimmedMessage || !this.receiverId) return;

    if (this.editMessage) {
      // Future extension: call API to update message
      this.editMessage.content = trimmedMessage;
      this.editMessage = null;
    } else {
      this.chatService.sendMessage(this.receiverId, trimmedMessage);
      this.messages.push({
        senderId: this.currentUserId,
        content: trimmedMessage,
        timestamp: new Date().toISOString()
      });
    }

    this.newMessage = '';
    this.scrollToBottom();
  }

  toggleMenu(msg: ChatMessageWithUI): void {
    this.messages.forEach(m => {
      if (m !== msg) m.showMenu = false;
    });
    msg.showMenu = !msg.showMenu;
  }

  startEditMessage(msg: ChatMessageWithUI): void {
    this.editMessage = msg;
    this.newMessage = msg.content || '';
    msg.showMenu = false;
  }

  deleteMessage(msg: ChatMessageWithUI): void {
    this.messages = this.messages.filter(m => m !== msg);
    // Optional: call API to delete the message from backend
  }

  private scrollToBottom(): void {
    try {
      if (this.messagesContainer) {
        this.messagesContainer.nativeElement.scrollTop =
          this.messagesContainer.nativeElement.scrollHeight;
      }
    } catch (err) {
      console.error('Scroll to bottom failed:', err);
    }
  }

  ngOnDestroy(): void {
    this.chatService.stopConnection?.();
  }

  getUser(userId: string): Observable<UserProfile> {
    return this.userService.getUserById(userId);
  }
}
