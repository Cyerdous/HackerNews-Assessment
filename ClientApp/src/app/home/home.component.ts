import { Component, inject } from '@angular/core';
import { ApiService } from '../api/api.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
	stories: Observable<number[]>;
	apiService: ApiService = inject(ApiService);

	constructor(){
		this.stories = this.apiService.getNewStories();
	}
}
