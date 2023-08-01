import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class LoggingService {
  logError(message: string, stack: string) {
    // Send errors to server here
    // Slack: https://medium.com/dailyjs/how-to-send-errors-into-slack-dc552e30506f
    //https://www.codemag.com/Article/1711021/Logging-in-Angular-Applications
    console.log('LoggingService: ' + message);
  }
}
