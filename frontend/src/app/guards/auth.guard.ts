import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const localStorageToken = localStorage.getItem('token');
  const sessionToken = sessionStorage.getItem('token');
  const token = localStorageToken || sessionToken;
  if (token) {
    return true;
  }

  return router.createUrlTree(['/login']);
};
