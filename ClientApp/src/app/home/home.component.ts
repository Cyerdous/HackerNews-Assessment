import { Component, inject, OnInit } from '@angular/core';
import { ApiService, NewsStory } from '../api/api.service';
import { BehaviorSubject, Observable, Subject, switchMap } from 'rxjs';
import { PageEvent } from '@angular/material/paginator';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {
	apiService: ApiService = inject(ApiService);
	page: Subject<number>;
	stories: Observable<NewsStory[]>;

	constructor(){
		this.page = new BehaviorSubject(0);
		this.stories = this.page.pipe(
			switchMap(page => this.apiService.getNewStories(page))
		);
	}

	ngOnInit(): void {
		this.page.next(0);
	}
	
	changeToPage(page: PageEvent): void {
		this.page.next(page.pageIndex);
	}
}
