import { Routes } from "@angular/router";
import { DashboardHomeComponent } from "./pages/dashboard-home/dashboard-home.component";
import { CreateDashboardComponent } from "./pages/create-dashboard/create-dashboard.component";
import { EditDashboardComponent } from "./pages/edit-dashboard/edit-dashboard.component";
import { DashboardOverviewComponent } from "./pages/dashboard-overview/dashboard-overview.component";

const routes: Routes = [
  {
    path: '',
    component: DashboardHomeComponent,
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
