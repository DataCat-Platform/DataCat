import { Routes } from "@angular/router";
import { CreateDashboardComponent } from "./pages/create-dashboard/create-dashboard.component";
import { EditDashboardComponent } from "./pages/edit-dashboard/edit-dashboard.component";
import { DashboardOverviewComponent } from "./pages/dashboard-overview/dashboard-overview.component";
import { DashboardListComponent } from "./pages/dashboard-list/dashboard-list.component";

const routes: Routes = [
  {
    path: '',
    component: DashboardListComponent,
  },
  {
    path: 'create',
    component: CreateDashboardComponent,
  },
  {
    path: 'edit/:id',
    component: EditDashboardComponent,
  },
  {
    path: 'overview/:id',
    component: DashboardOverviewComponent,
  },
];

export default routes;
