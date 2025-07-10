export interface ChatMessage{
  id: number;
  senderId: string;
  receiverId: string;
  content: string;
  timestamp?: string;   
}