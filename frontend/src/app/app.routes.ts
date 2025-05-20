import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { RegistrarComponent } from './pages/registrar/registrar.component';
import { ListagemPedidosComponent } from './pages/dashboard/listagem-pedidos/listagem-pedidos.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
    {component: LoginComponent, path: 'login'},
    {component:RegistrarComponent,path:'registrar'},
    {
      component:ListagemPedidosComponent,
      path:'dashboard/pedidos',
      canActivate:[authGuard]
    }
    
];
