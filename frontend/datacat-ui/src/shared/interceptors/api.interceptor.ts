import { HttpInterceptorFn } from '@angular/common/http';
import { environment } from '../env/environment';

export const apiInterceptor: HttpInterceptorFn = (req, next) => {
  const baseUrl = environment.apiUrl;
  const apiReq = req.clone({ url: `${baseUrl}${req.url}` });
  return next(apiReq);
};
