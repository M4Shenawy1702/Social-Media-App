import { PostComment } from "./PostComment";
import { PostMediaDetails } from "./PostMediaDetails";

export interface Post {
    id: number;
    content: string;
    authorId: string;
    authorName: string;
    authorImageUrl: string;
    media: PostMediaDetails[];
    comments: PostComment[];
    likes: number;
}

