import { Component, inject } from '@angular/core';
import { NgForOf, NgOptimizedImage } from "@angular/common";
import { Router, RouterLink } from "@angular/router";
import { NgIcon, provideIcons } from "@ng-icons/core";
import { heroHome, heroCircleStack, heroTableCells } from '@ng-icons/heroicons/outline';

import { AuthService } from "../../../core/auth/auth.service";
import { NavigationModel } from "../../models/navigation.model";

@Component({
  selector: 'app-sidebar',
  imports: [
    NgForOf,
    NgOptimizedImage,
    NgIcon,
    RouterLink,
  ],
  providers: [
      provideIcons({ heroHome, heroCircleStack, heroTableCells })
  ],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent {
  public readonly router: Router = inject(Router);
  public readonly authService: AuthService = inject(AuthService);

  public readonly navigations: NavigationModel[] = [
    {
      name: "Home",
      route: "/",
      icon: heroHome,
    },
    {
      name: "Rooms",
      route: "/rooms",
      icon: heroCircleStack,
    },
    {
      name: "Leaderboard",
      route: "/leaderboard",
      icon: heroTableCells,
    }
  ];

  public mobileSidebarEnabled: boolean = false;

  public toggleMobileSidebar(): void {
    this.mobileSidebarEnabled = !this.mobileSidebarEnabled;
  }
}
