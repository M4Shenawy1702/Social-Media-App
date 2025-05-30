export interface PostComment {
    id: number;
    content: string;
    authorName: string;
    authorId: string;
    lastUpdatedAt: Date;
    authorImageUrl ?: string;
    postId : number;
}