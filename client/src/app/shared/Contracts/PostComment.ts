export interface PostComment {
    id: number;
    content: string;
    authorName: string;
    lastUpdatedAt: Date;
    authorImageUrl ?: string;
}