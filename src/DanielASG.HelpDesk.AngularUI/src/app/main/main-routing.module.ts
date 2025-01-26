import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    {
                        path: 'home',
                        loadChildren: () => import('./home/home.module').then((m) => m.HomeModule),
                        data: { permission: 'Pages.Tenant.Home' },
                    },
                    {
                        path: 'tickets/tickets',
                        loadChildren: () => import('./tickets/tickets/ticket.module').then(m => m.TicketModule),
                        data: { permission: 'Pages.Tickets' }
                    },
                
                    
                    {
                        path: 'departments/departments',
                        loadChildren: () => import('./departments/departments/department.module').then(m => m.DepartmentModule),
                        data: { permission: 'Pages.Departments' }
                    },
                
                    {
                        path: 'dashboard',
                        loadChildren: () => import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
                        data: { permission: 'Pages.Tenant.Dashboard' },
                    },
                    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
                    { path: '**', redirectTo: 'dashboard' },
                ],
            },
        ]),
    ],
    exports: [RouterModule],
})
export class MainRoutingModule {}
