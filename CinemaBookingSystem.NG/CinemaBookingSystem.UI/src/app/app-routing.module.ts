import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { ProvinceListComponent } from './components/province/province-list/province-list.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {path:'province-list' , component: ProvinceListComponent}, 
  {path:'login' , component: LoginComponent},
  {path:'home' , component: HomeComponent},
  { path: '',   redirectTo: '/home', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
