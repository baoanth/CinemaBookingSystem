import { CarouselEditComponent } from './components/carousel/carousel-edit/carousel-edit.component';
import { CarouselAddComponent } from './components/carousel/carousel-add/carousel-add.component';
import { CarouselListComponent } from './components/carousel/carousel-list/carousel-list.component';
import { MovieDetailComponent } from './components/movie/movie-detail/movie-detail.component';
import { MovieEditComponent } from './components/movie/movie-edit/movie-edit.component';
import { MovieAddComponent } from './components/movie/movie-add/movie-add.component';
import { MovieListComponent } from './components/movie/movie-list/movie-list.component';
import { ProvinceAddComponent } from './components/province/province-add/province-add.component';
import { ProvinceListComponent } from './components/province/province-list/province-list.component';
import { HomeComponent } from './components/home/home.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { ProvinceEditComponent } from './components/province/province-edit/province-edit.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { SupportOnlineListComponent } from './components/support-online/support-online-list/support-online-list.component';
import { SupportOnlineAddComponent } from './components/support-online/support-online-add/support-online-add.component';
import { SupportOnlineEditComponent } from './components/support-online/support-online-edit/support-online-edit.component';

const routes: Routes = [
  { path: '',   redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'movie', component: MovieListComponent },
  { path: 'movie/add', component: MovieAddComponent },
  { path: 'movie/edit/:id', component: MovieEditComponent },
  { path: 'movie/detail/:id', component: MovieDetailComponent },
  { path: 'province', component: ProvinceListComponent },
  { path: 'province/add', component: ProvinceAddComponent },
  { path: 'province/edit/:id', component: ProvinceEditComponent },
  { path: 'support', component: SupportOnlineListComponent },
  { path: 'support/add', component: SupportOnlineAddComponent },
  { path: 'support/edit/:id', component: SupportOnlineEditComponent },
  { path: 'carousel', component: CarouselListComponent },
  { path: 'carousel/add', component: CarouselAddComponent },
  { path: 'carousel/edit/:id', component: CarouselEditComponent },
  { path: '**', pathMatch: 'full', 
        component: PageNotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
