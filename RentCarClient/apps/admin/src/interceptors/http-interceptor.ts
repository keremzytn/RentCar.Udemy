import { HttpInterceptorFn } from '@angular/common/http';

export const httpInterceptor: HttpInterceptorFn = (req, next) => {
  const url = req.url;
  const endpoint = 'https://localhost:5000/';
  let clone = req.clone({
    url: url.replace('/rent/', endpoint)
  });
  return next(clone);
};
