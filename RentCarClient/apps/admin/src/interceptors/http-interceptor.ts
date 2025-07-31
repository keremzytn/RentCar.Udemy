import { HttpInterceptorFn } from '@angular/common/http';

export const httpInterceptor: HttpInterceptorFn = (req, next) => {
  const url = req.url;
  const endpoint = 'http://localhost:5000/';
  let clone = req.clone({
    url: !url.startsWith('http') ? endpoint + url : url
  });
  return next(clone);
};