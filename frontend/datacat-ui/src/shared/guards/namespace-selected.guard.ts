import { inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  RouterStateSnapshot,
} from '@angular/router';
import { UserService } from '../services/user.service';

export const namespaceSelectedGuardFn: CanActivateFn = (
  next: ActivatedRouteSnapshot,
  state: RouterStateSnapshot,
) => {
  let userService = inject(UserService);

  if (userService.namespace) {

  }

  return true;
};
