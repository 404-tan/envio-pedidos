import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { RegistrarComponent } from './pages/registrar/registrar.component';

export const routes: Routes = [
    {component: LoginComponent, path: 'login'},
    {component:RegistrarComponent,path:'registrar'}
];
