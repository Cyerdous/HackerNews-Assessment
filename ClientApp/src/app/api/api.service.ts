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

	public getNewStories(): Observable<number[]> {
		return this.http.get<number[]>(this.baseUrl + 'api/HackerNews/GetHackerNewsNewStories')
	}
}
