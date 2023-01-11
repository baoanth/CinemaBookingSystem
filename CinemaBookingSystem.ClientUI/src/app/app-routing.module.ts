import { ProvinceAddComponent } from './components/province/province-add/province-add.component';
import { ProvinceListComponent } from './components/province/province-list/province-list.component';
import { HomeComponent } from './components/home/home.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { ProvinceEditComponent } from './components/province/province-edit/province-edit.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';

const routes: Routes = [
  { path: '',   redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'province', component: ProvinceListComponent },
  { path: 'province/add', component: ProvinceAddComponent },
  { path: 'province/edit/:id', component: ProvinceEditComponent },
  { path: '**', pathMatch: 'full', 
        component: PageNotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
