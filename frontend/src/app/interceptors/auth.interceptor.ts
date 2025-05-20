
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const localStoragetoken = localStorage.getItem('token');
  const sessionToken = sessionStorage.getItem('token');
  const token = localStoragetoken || sessionToken;
  const authReq = token
    ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
    : req;

  return next(authReq).pipe(
    catchError(err => {
      if (err.status === 401) {

        localStorage.removeItem('token');
        router.navigate(['/login']);
      }

      return throwError(() => err);
    })
  );
};
