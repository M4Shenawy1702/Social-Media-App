import { Component, OnInit, OnDestroy, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ChatSignalrService } from '../Services/chat-signal-r.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ChatMessage } from '../shared/Contracts/ChatMessage';
import { UserProfile } from '../shared/Contracts/UserProfile';
import { UserService } from '../Services/user.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-chat',
  imports: [FormsModule, CommonModule],
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit, OnDestroy, AfterViewChecked {
  messages: ChatMessage[] = [];
  newMessage = '';
  receiverId = '';
  receiverName = '';
  currentUserId = '';
  user: UserProfile | null = null;
 baseUrl = 'http://localhost:5043/';

  @ViewChild('messagesContainer') private messagesContainer!: ElementRef;

  constructor(private chatService: ChatSignalrService, private route: ActivatedRoute, private userService: UserService) { }

  ngOnInit(): void {
    this.receiverId = this.route.snapshot.paramMap.get('id') || '';
    this.receiverName = this.route.snapshot.paramMap.get('name') || '';
    const user = localStorage.getItem('user');

    this.userService.getUserById(this.receiverId).subscribe({
      next: (profile) => {
        this.user = profile;
      },
      error: (err) => {
        console.error('Failed to load user profile:', err);
      }
    });

    if (user) {
      const userObj = JSON.parse(user);
      const token = userObj.token;
      this.currentUserId = userObj.userId;

      this.chatService.startConnection(token);

      // Load old messages
      this.chatService.getMessages(this.receiverId, (msgs) => {
        this.messages = msgs;
        this.scrollToBottom();
      });

      // Listen for new messages
      this.chatService.onMessageReceived((senderId, message) => {
        this.messages.push({ senderId, content: message, timestamp: new Date().toISOString() });
        this.scrollToBottom();
      });

    } else {
      console.error('User not logged in or token missing');
    }
  }

  ngAfterViewChecked(): void {
    this.scrollToBottom();
  }

  sendMessage(): void {
    if (this.newMessage.trim() && this.receiverId.trim()) {
      this.chatService.sendMessage(this.receiverId, this.newMessage);
      this.messages.push({ senderId: this.currentUserId, content: this.newMessage, timestamp: new Date().toISOString() });
      this.newMessage = '';
      this.scrollToBottom();
    }
  }

  private scrollToBottom(): void {
    try {
      if (this.messagesContainer) {
        this.messagesContainer.nativeElement.scrollTop = this.messagesContainer.nativeElement.scrollHeight;
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
