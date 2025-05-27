import { PostMediaDetails } from "./PostMediaDetails";

export interface CreatePost{
    content: string;
    authorId: string;
    media: PostMediaDto[];
}

interface PostMediaDto
{
    url: File;
    type: string;
}