import { Injectable } from "@angular/core";
import * as signalR from "@microsoft/signalr";
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

interface ChatMessage {
  senderId: string;
  content: string;
  timestamp?: string;
}

@Injectable({
  providedIn: 'root'
})
export class ChatSignalrService {
  private hubConnection!: signalR.HubConnection;
  private readonly baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) {}

  startConnection(token?: string): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.baseUrl}/chathub`, {
        accessTokenFactory: () => {
          if (token) return token;
          const user = localStorage.getItem('user');
          return user ? JSON.parse(user).token : null;
        }
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start()
      .then(() => console.log('Connected!'))
      .catch(err => console.error('Connection error:', err));
  }

  sendMessage(receiverId: string, message: string): void {
    this.hubConnection.invoke('SendMessage', receiverId, message)
      .catch(err => console.error('SendMessage error:', err));
  }

  onMessageReceived(callback: (senderId: string, message: string) => void): void {
    this.hubConnection.on('ReceiveMessage', callback);
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
