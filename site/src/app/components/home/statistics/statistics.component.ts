import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss']
})
export class StatisticsComponent implements OnInit {

  
  constructor(private apiService: ApiService) { 

    this.apiService.getGuildsCount().subscribe(result => {
      this.servers = {
        countTo:  result.data,
        from: 0,
        duration: 1
      };
    });

    this.apiService.getSubredditsCount().subscribe(result => {
      this.subreddits = {
        countTo:  result.data,
        from: 0,
        duration: 1
      };
    })
  }

  servers = {
    countTo: 0,
    from: 0,
    duration: 0.1
  };
  
  subreddits = {
    countTo: 0,
    from: 0,
    duration: 0.1
  };

  ngOnInit(): void {
  }

}
