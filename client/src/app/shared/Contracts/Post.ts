import { PostComment } from "./PostComment";

export interface Post {
    id: number;
    content: string;
    authorId: string;
    authorName: string;
    authorImageUrl: string;
    mediaUrl: string;
    comments: PostComment[];
    likes: number;
    isLiked: boolean;
}

