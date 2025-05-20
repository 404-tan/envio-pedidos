
import { definePreset } from '@primeng/themes';
import Lara from '@primeng/themes/lara';

const LaraLightRed = definePreset(Lara, {
  semantic: {
    primary: {
      50: '{red.50}',
      100: '{red.100}',
      200: '{red.200}',
      300: '{red.300}',
      400: '{red.400}',
      500: '{red.500}',
      600: '{red.600}',
      700: '{red.700}',
      800: '{red.800}',
      900: '{red.900}',
      950: '{red.950}'
    },
    surface: {
      0: '{gray.0}',       // branco total
      50: '{gray.50}',
      100: '{gray.100}',
      200: '{gray.200}',
      300: '{gray.300}',
      400: '{gray.400}',
      500: '{gray.500}',
      600: '{gray.600}',
      700: '{gray.700}',
      800: '{gray.800}',
      900: '{gray.900}',
      950: '{gray.950}'
    },
    content: {
      0: '{gray.950}',    // texto escuro
      50: '{gray.900}',
      100: '{gray.800}',
      200: '{gray.700}',
      300: '{gray.600}',
      400: '{gray.500}',
      500: '{gray.400}',
      600: '{gray.300}',
      700: '{gray.200}',
      800: '{gray.100}',
      900: '{gray.50}',
      950: '{gray.0}'
    }
  }
});

export default LaraLightRed;
