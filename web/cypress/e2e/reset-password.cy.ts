/* eslint-disable prettier/prettier */
describe('Reset password', () => {
  it('Apply an email to reset password', () => {
    cy.visit('/account/reset-password');
    cy.url().should('contain', '/by-email', 'First you should open by-email page');

    cy.contains('Сброс пароля');

    cy.get('.notification').should('contain', 'Введите адрес электронной почты');

    cy.get('input[type=email]').should('be.visible');
    cy.get('button[type=submit]').should('be.visible').should('contain.text', 'Отправить');
    cy.get('input[type=email]').type('test');
    cy.get('button[type=submit]').click();
    cy.get('div.is-danger').should('be.visible');
  });

  it('Check email page validators', () => {
    cy.visit('/account/reset-password/check-email');

    cy.get('div[role=alert]').should('contain.text', 'Неверная email');
    cy.url().should('not.contain', '/check-email');

    cy.get('div[role=alert]').click();
    cy.get('div[role=alert]').should('not.exist');

    cy.get('input[type=email]').type('abbos@mail.ru');
    cy.get('div.is-danger').should('not.exist');

    cy.intercept('post', '/api/account/forgotpassword', { delay: 1000 }).as('forgotPassword');
    cy.get('button[type=submit]').click();
    cy.get('button[type=submit]').should('not.exist');
    cy.get('progress').should('be.visible');
    cy.wait('@forgotPassword');
    cy.url().should('contain', 'check-email?email=abbos@mail.ru');
  });

  it('Apply password has wrong ling', () => {
    cy.visit('/account/reset-password/apply');
    cy.url().should('not.contain', '/apply');
    cy.get('div[role=alert]').should('contain.text', 'Неверная ссылка');
    cy.get('div[role=alert]').click();
  });

  it('Apply password tests', () => {
    cy.visit('/account/reset-password/apply?token=ajsnsef234wer&email=abbos@mail.ru');

    cy.contains('Восстановление пароля');
    cy.get('input[formcontrolname=newPassword]').should('exist');
    cy.get('input[formcontrolname=confirmPassword]').should('exist');
    cy.get('button[type=submit]').should('contain.text', 'Восстановить');
    cy.get('input[formcontrolname=newPassword]').type('test');
    cy.get('button[type=submit]').click();
    cy.get('input[formcontrolname=newPassword] + div').should('contain.text', 'Пароль должен состоять минимум из');
    cy.get('input[formcontrolname=confirmPassword] + div').should('contain.text', 'Не совпадает с текущим паролем');
    cy.get('input[formcontrolname=confirmPassword]').type('test');
    cy.get('input[formcontrolname=confirmPassword] + div').should('not.exist');

    cy.get('input[formcontrolname=newPassword]').type('Test1234$');
    cy.get('input[formcontrolname=confirmPassword]').type('Test1234$');
    cy.get('div.is-danger').should('not.exist');

    //request
    cy.intercept('post', '/api/account/resetPassword', { delay: 1000 }).as("resetPassword");
    cy.get('button[type=submit]').click();
    cy.get('button[type=submit]').should('not.exist');
    cy.get('progress').should('be.visible');
    cy.wait('@resetPassword');
    cy.url().should('contain', '/login');
    cy.get('div[role=alert]').should('contain.text', 'успешно изменен');
  });
});
