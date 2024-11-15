import pt from '@angular/common/locales/pt';

import {
  ApplicationConfig,
  provideZoneChangeDetection,
  importProvidersFrom
}
from '@angular/core';

import {
  API_URL,
  API_VERSION,
  APPLICATION_TITLE
}
from './injection.tokens';

import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideNzI18n, pt_BR } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient } from '@angular/common/http';
import { provideNzIcons } from './icons-provider';
import { NzModalService } from 'ng-zorro-antd/modal';
import { environment } from '../environments/environment';
import { DemoNgZorroAntdModule } from './ng-zorro-antd.module';
import { DateFormatPipe } from './pipes/dateFormat.pipe';

registerLocaleData(pt);

export const appConfig: ApplicationConfig = {
  providers: [
    NzModalService,
    { provide: API_URL, useValue: environment.api },
    { provide: API_VERSION, useValue: environment.apiVersion },
    { provide: APPLICATION_TITLE, useValue: environment.applicationTitle },

    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideClientHydration(),
    provideNzIcons(),
    provideNzI18n(pt_BR),

    importProvidersFrom(
      FormsModule,
      ReactiveFormsModule,
      DemoNgZorroAntdModule
    ),

    provideAnimationsAsync(),
    provideHttpClient(),
    DateFormatPipe
  ]
};
