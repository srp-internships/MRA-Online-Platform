import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { GuideExercisesComponent } from './exercises.component';

@Injectable()
export default class ExerciseDeactivateGuard implements CanDeactivate<GuideExercisesComponent> {
  canDeactivate(component: GuideExercisesComponent) {
    const flag = component.hasUserChanges();
    if (flag) {
      return confirm('Если вы покинете, ваши изменения будут отменены!');
    }
    return true;
  }
}
