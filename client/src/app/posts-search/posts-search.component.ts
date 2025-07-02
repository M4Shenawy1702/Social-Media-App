import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PostsService } from './../Services/posts.service';
import { Post } from './../shared/Contracts/Post';
import { PostQueryParameters } from '../shared/Contracts/PostQueryParameters';
import { PostsComponent } from "../posts/posts.component";
import { PagenatedResult } from '../shared/Contracts/PagenatedResult';

@Component({
  selector: 'app-posts-search',
  templateUrl: './posts-search.component.html',
  styleUrls: ['./posts-search.component.scss'],
  imports: [PostsComponent]
})
export class PostsSearchComponent implements OnInit {
  searchTerm: string = '';
  posts: PagenatedResult<Post> = {
    data: [],
    count: 0,
    pageIndex: 1,
    pageCount: 10
  };

  constructor(private route: ActivatedRoute, private postsService: PostsService) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.searchTerm = params.get('searchTerm') || '';
      this.postsSearch();
    });
  }


  postsSearch() {
    const params: PostQueryParameters = {
      pageIndex: 1,
      pageSize: 10,
      search: this.searchTerm
    };

    this.postsService.getAllPosts(params).subscribe({
      next: (res) => {
        this.posts = res;
      },
      error: (err) => {
        console.error('Error fetching posts:', err);
      }
    });
  }
}
