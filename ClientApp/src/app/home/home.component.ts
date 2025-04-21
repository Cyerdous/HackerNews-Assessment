import { Component, inject, OnInit } from '@angular/core';
import { ApiService, NewsStory } from '../api/api.service';
import { BehaviorSubject, Observable, Subject, switchMap, merge, startWith} from 'rxjs';
import { PageEvent } from '@angular/material/paginator';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {
	apiService: ApiService = inject(ApiService);
	getStories: Subject<number>;
	getStoryCount: Subject<string>;
	stories: Observable<NewsStory[]>;
	storyCount: Observable<number>;

	page: number;
	query: string;

	constructor(){
		this.page = 0;
		this.query = "";

		this.getStoryCount = new Subject()
		this.getStories = new BehaviorSubject(0);

		this.stories = merge(
			this.getStories.pipe(
				switchMap(page => this.apiService.getNewStories(page, this.query))
			),
			this.getStoryCount.pipe(
				switchMap(query => this.apiService.getNewStories(this.page, query))
			)
		);

		this.storyCount = this.getStoryCount.pipe(
			switchMap(query => this.apiService.getStoryCount(query)),
			startWith(500)
		);

	}

	ngOnInit(): void {
	}
	
	changeToPage(page: PageEvent): void {
		this.page = page.pageIndex;
		this.getStories.next(this.page);
	}

	onQueryChange(query: string): void {
		this.page = 0
		this.query = query
		this.getStoryCount.next(this.query);
	}
}
