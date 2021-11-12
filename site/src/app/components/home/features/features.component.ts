import { Component, OnInit } from '@angular/core';
import { Feature } from 'src/app/data/feature';

@Component({
  selector: 'app-features',
  templateUrl: './features.component.html',
  styleUrls: ['./features.component.scss']
})
export class FeaturesComponent implements OnInit {

  features : Feature[] = [
    {name : 'Custom Playlists', description: "use playlists", icon: 'list-ul'},
    {name : 'Subscriptions', description: "use playlists", icon: 'person-lines-fill'},
    {name : 'Lightning fast', description: "use playlists", icon: 'lightning-fill'},
    {name : 'Easy to use', description: "use playlists", icon: 'lightning-fill'},
    {name : 'Ban subreddits', description: "use playlists", icon: 'shield-fill-x'},
    {name : 'Highly configurable', description: "use playlists", icon: 'gear-fill'},
  ]
  constructor() { }

  ngOnInit(): void {
  }

}
