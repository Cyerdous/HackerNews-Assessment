import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing'
import { ApiService } from './api.service';
import { provideHttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';

const data = [{"id":43748256,"by":"martialg","time":1745205316,"url":"https://www.nytimes.com/2025/04/19/opinion/extinction-technology-culture.html","title":"An Age of Extinction Is Coming"},{"id":43748234,"by":"misrasaurabh1","time":1745204966,"url":"https://www.codeflash.ai/post/llms-struggle-to-write-performant-code","title":"LLMs struggle to write performant code"},{"id":43748232,"by":"Arindam1729","time":1745204956,"url":"https://news.ycombinator.com/item?id=43748232","title":"The most complete (and easy) explanation of MCP vulnerabilities"},{"id":43748225,"by":"Arindam1729","time":1745204900,"url":"https://news.ycombinator.com/item?id=43748225","title":"OpenAI's new enterprise AI guide is a goldmine for real-world adoption"},{"id":43748216,"by":"vyrotek","time":1745204824,"url":"https://www.highnoongame.com/post/the-boardgame-industry-is-burning","title":"The Board Game Industry Is Burning – and It's Their Own Fault"},{"id":43748174,"by":"evenoroddman","time":1745204278,"url":"https://news.ycombinator.com/item?id=43748174","title":"Ask HN: Built a dev tool, but struggling with traction – advice?"},{"id":43748172,"by":"wahnfrieden","time":1745204242,"url":"https://www.androidpolice.com/skip-interview/","title":"How the development wall between Android and iOS may soon come down – Skip"},{"id":43748171,"by":"MilnerRoute","time":1745204235,"url":"https://www.newcartographies.com/p/epistemological-slop","title":"'Epistemological Slop: Lies, Damned Lies, and Google'"},{"id":43748133,"by":"qingcharles","time":1745203675,"url":"https://www.youtube.com/watch?v=aUOnQ_boqCw","title":"I thought I bought a camera, but no DJI sold me a LICENSE to use their camera [video]"},{"id":43748117,"by":"Dropbysometime","time":1745203520,"url":"https://www.readtrung.com/p/the-ludicrous-psychology-of-slot","title":"The (Ludicrous) Psychology of Slot Machines"},{"id":43748113,"by":"petethomas","time":1745203455,"url":"https://www.reuters.com/world/us/defense-chief-hegseth-shared-war-plans-second-signal-chat-nyt-reports-2025-04-20/","title":"Pentagon chief shared Yemen war plans in second Signal chat"},{"id":43748111,"by":"mrcsharp","time":1745203433,"url":"https://github.com/dotnet/core/blob/main/release-notes/10.0/preview/preview3/csharp.md","title":"C# 14 updates in .NET 10 Preview 3 – Release Notes"},{"id":43748107,"by":"yieldalarm","time":1745203380,"url":"https://lookbusy.app","title":"Show HN: Look Busy – Realistic-Looking Fake Calendar Events"},{"id":43748096,"by":"misdiva1bil","time":1745203273,"url":"https://play.google.com/store/apps/details?id=com.aimixfour","title":"AI Mix"},{"id":43748094,"by":"zeech","time":1745203258,"url":"https://yugologo.org/","title":"Yugologo – an archive of business' logos from the former Yugoslavia"},{"id":43748086,"by":"maowtm","time":1745203159,"url":"https://blog.maowtm.org/linux-ick/en.html","title":"Using the Linux kernel to help me crack an executable quickly"},{"id":43748083,"by":"bikedspiritlake","time":1745203134,"url":"https://threads.net/@wongmjane/post/DIos1OSpG3q","title":"Figma Sites: Design responsively, then launch with a click"},{"id":43748047,"by":"x8n","time":1745202634,"url":"https://youtu.be/5Q2iNGhI3KA","title":"AP: Humanoid robots run a half-marathon in China alongside humans"},{"id":43748025,"by":"benbreen","time":1745202260,"url":"https://www.newyorker.com/culture/the-weekend-essay/mistaking-mary-magdalene","title":"Mistaking Mary Magdalene"},{"id":43748023,"by":"daly","time":1745202234,"url":"https://news.ycombinator.com/item?id=43748023","title":"Robots making Lego kits without prior training?"}]

describe('ApiService', () => {
	let service: ApiService;
	let httpTesting: HttpTestingController

	beforeEach(() => {
		TestBed.configureTestingModule({
			providers: [
				ApiService,
				provideHttpClient(),
				provideHttpClientTesting(),
				{
					provide: 'BASE_URL',
					useValue: ""
				}
			]
		});
		
		httpTesting = TestBed.inject(HttpTestingController);
		service = TestBed.inject(ApiService);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});

	it('should retrieve stories', async () => {
		const stories$ = service.getNewStories(0,"");
		const storiesPromise = firstValueFrom(stories$);
		const req = httpTesting.expectOne('api/HackerNews/GetNewsByPage/0/');
		expect(req.request.method).toBe('GET');
		req.flush(data)
		expect(await storiesPromise).toEqual(data)
	});

	it('should retrieve story count', async () => {
		const stories$ = service.getStoryCount("");
		const storiesPromise = firstValueFrom(stories$);
		const req = httpTesting.expectOne('api/HackerNews/GetStoryCount/');
		expect(req.request.method).toBe('GET');
		req.flush(500)
		expect(await storiesPromise).toEqual(500)
	});
});
