import { HttpClient } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';

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

	public getNewStories(): Observable<NewsStory[]> {
		return this.http.get<NewsStory[]>(this.baseUrl + 'api/HackerNews/GetNewStories')
	}
}

export interface NewsStory {
	id: number;
	by: string;
	time: number;
	url: string;
	title: string;
}
