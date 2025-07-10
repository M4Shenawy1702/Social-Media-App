import {
  Component,
  OnInit,
  OnDestroy,
  ViewChild,
  ElementRef,
  AfterViewChecked
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { ChatSignalrService } from '../Services/chat-signal-r.service';
import { UserService } from '../Services/user.service';

import { ChatMessage } from '../shared/Contracts/ChatMessage';
import { UserProfile } from '../shared/Contracts/UserProfile';
import Swal from 'sweetalert2';

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
    private userService: UserService,
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

      this.chatService.onMessageReceived((msg) => {
        const newMessage: ChatMessageWithUI = {
          ...msg,
          timestamp: msg.timestamp || new Date().toISOString()
        };

        this.messages.push(newMessage);
        this.scrollToBottom();
      });

      this.chatService.onMessageUpdated((updatedMessage) => {
        const index = this.messages.findIndex(m => m.id === updatedMessage.id);
        if (index > -1) {
          this.messages[index].content = updatedMessage.content;

        }
      });

      this.chatService.onMessageDeleted(({ messageId }) => {
        this.messages = this.messages.filter(m => m.id !== messageId);
      });
    });
  }

  ngAfterViewChecked(): void {
    this.scrollToBottom();
  }

  sendMessage(): void {
    const trimmedMessage = this.newMessage.trim();
    if (!trimmedMessage || !this.receiverId) return;
    else {
      this.chatService.sendMessage(this.receiverId, trimmedMessage);
    }
    this.newMessage = '';
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
  oneditMessage(): void {
    if (!this.editMessage) return;

    const trimmedMessage = this.newMessage.trim();
    if (!trimmedMessage) return;

    this.chatService.updateMessage(this.editMessage.id, trimmedMessage);

    const index = this.messages.findIndex(m => m.id === this.editMessage?.id);
    if (index > -1) {
      this.messages[index].content = trimmedMessage;
    }

    this.editMessage = null;
    this.newMessage = '';
  }
  cancelEdit(): void {
    this.editMessage = null;
    this.newMessage = '';
  }
  deleteMessage(msgId: number): void {
    Swal.fire({
      title: 'Are you sure?',
      text: 'You will not be able to recover this message!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'No, keep it'
    }).then((result) => {
      if (result.isConfirmed) {

        this.messages = this.messages.filter(m => m.id !== msgId);

        if (msgId != null) {
          this.chatService.deleteMessage(msgId);
        }
      }
    });
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
