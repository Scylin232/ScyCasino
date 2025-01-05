import {Component} from '@angular/core';

import {DataTableComponent} from '../../shared/components/data-table/data-table.component';

import {User} from '../../shared/models/user.model';

@Component({
  selector: 'app-leaderboard',
  imports: [
    DataTableComponent
  ],
  templateUrl: './leaderboard.component.html',
  styleUrl: './leaderboard.component.css'
})
export class LeaderboardComponent {
  public leaderboardEntryMappings: { name: string, key: keyof User }[] = [
    {
      name: "Name",
      key: "nickname"
    },
    {
      name: "Coins",
      key: "coins"
    },
    {
      name: "Joined",
      key: "createdAt"
    }
  ];
}
