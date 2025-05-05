import { Component, OnDestroy } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { filter, Subscription } from 'rxjs';

@Component({
  standalone: true,
  selector: 'datacat-activity-breadcrumb',
  templateUrl: 'activity-breadcrumb.component.html',
  imports: [BreadcrumbModule],
})
export class ActivityBreadcrumbComponent implements OnDestroy {
  constructor(private _router: Router) {
    this.setUpUpdateOnRouteChange();
  }

  private _subscription?: Subscription;
  protected pathToActivityMenuItems: MenuItem[] = [];

  private setUpUpdateOnRouteChange() {
    this._subscription = this._router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event) => {
        this.pathToActivityMenuItems = (event as NavigationEnd).url
          .split('/')
          .filter((u: string) => u.length !== 0)
          .map((u: string) => {
            return {
              label: u,
            };
          });
      });
  }

  ngOnDestroy() {
    this._subscription?.unsubscribe();
  }
}
