import { HttpClient } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';
import { Observable, retry } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class ApiService {
	private baseUrl: string
	private http: HttpClient


	constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
		this.baseUrl = baseUrl;
		this.http = http
	}

	public getNewStories(page: number, query: string): Observable<NewsStory[]> {
		return this.http.get<NewsStory[]>(this.baseUrl + `api/HackerNews/GetNewsByPage/${page}/${query}`);
	}

	public getStoryCount(query: string): Observable<number> {
		return this.http.get<number>(this.baseUrl + `api/HackerNews/GetStoryCount/${query}`);
	}
}

export interface NewsStory {
	id: number;
	by: string;
	time: number;
	url: string;
	title: string;
}
