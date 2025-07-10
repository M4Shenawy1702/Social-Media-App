import { Injectable } from "@angular/core";
import * as signalR from "@microsoft/signalr";
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

interface ChatMessage {
  id: number;
  senderId: string;
  receiverId: string;
  content: string;
  timestamp?: string;
  chatId?: number;
}

@Injectable({
  providedIn: 'root'
})
export class ChatSignalrService {
  private hubConnection!: signalR.HubConnection;
  private readonly baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  startConnection(token?: string): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.baseUrl}/chathub`, {
        accessTokenFactory: () => {
          if (token) return token;
          const user = localStorage.getItem('user');
          return user ? JSON.parse(user).token : null;
        },
        transport: signalR.HttpTransportType.WebSockets 
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start()
      .then(() => console.log('SignalR Connected!'))
      .catch(err => console.error('SignalR Connection Error:', err));
  }

  sendMessage(receiverId: string, message: string): void {
    this.hubConnection.invoke('SendMessage', receiverId, message)
      .catch(err => console.error('SendMessage error:', err));
  }

  onMessageReceived(callback: (message: ChatMessage) => void): void {
    this.hubConnection.on('ReceiveMessage', (msg: ChatMessage) => {
      callback(msg);
    });
  }

  onMessageUpdated(callback: (updatedMessage: ChatMessage) => void): void {
    this.hubConnection.on('MessageUpdated', (msg: ChatMessage) => {
      callback(msg);
    });
  }

  onMessageDeleted(callback: (deletedMessage: { messageId: number }) => void): void {
    this.hubConnection.on('MessageDeleted', callback);
  }

  onChatDeleted(callback: (chatId: number) => void): void {
    this.hubConnection.on('ChatDeleted', callback);
  }

  getMessages(receiverId: string, callback: (msgs: ChatMessage[]) => void): void {
    const token = localStorage.getItem('jwtToken') || '';

    this.http.get<ChatMessage[]>(`${this.baseUrl}/api/chat/${receiverId}`, {
      headers: { Authorization: `Bearer ${token}` }
    }).subscribe({
      next: msgs => callback(msgs),
      error: err => console.error('Failed to load messages', err)
    });
  }

  deleteMessage(messageId: number): void {
    const token = localStorage.getItem('jwtToken') || '';
    this.http.delete(`${this.baseUrl}/api/chat/${messageId}`, {
      headers: { Authorization: `Bearer ${token}` }
    }).subscribe({
      next: () => console.log('Message deleted'),
      error: err => console.error('Delete message error:', err)
    });
  }

  updateMessage(messageId: number, content: string): void {
    const token = localStorage.getItem('jwtToken') || '';
    this.http.put(`${this.baseUrl}/api/chat/${messageId}`, { content }, {
      headers: { Authorization: `Bearer ${token}` }
    }).subscribe({
      next: () => console.log('Message updated'),
      error: err => console.error('Update message error:', err)
    });
  }

  deleteChat(chatId: number): void {
    const token = localStorage.getItem('jwtToken') || '';
    this.http.delete(`${this.baseUrl}/api/chat/by-chat/${chatId}`, {
      headers: { Authorization: `Bearer ${token}` }
    }).subscribe({
      next: () => console.log('Chat deleted'),
      error: err => console.error('Delete chat error:', err)
    });
  }

  async stopConnection(): Promise<void> {
    if (this.hubConnection) {
      try {
        await this.hubConnection.stop();
        console.log('Connection stopped');
      } catch (err) {
        console.error('Error stopping connection:', err);
      }
    }
  }
}
