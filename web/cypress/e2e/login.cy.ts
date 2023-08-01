describe('Login page', () => {
  beforeEach(() => {
    cy.visit('/account/login');
  });

  it('Check page contents', () => {
    cy.contains('MRA Academy');
    cy.contains('Email');
    cy.get('input[type=email]').should('be.visible');
    cy.contains('Пароль');
    cy.get('input[type=password]').should('be.visible');
    cy.get('a[href="/account/reset-password"]').should('contain.text', 'Забыли пароль?');
    cy.get('button[type=submit]').should('be.visible').contains('Войти');
  });

  it('Field validations', () => {
    cy.get('button[type=submit]').click();
    cy.contains('Требуется электронная почта.');
    cy.contains('Требуется пароль.');

    cy.get('input[type=email]').type('asdsa');
    cy.contains('Требуется электронная почта.').should('not.exist');
    cy.contains('Введите действительный адрес электронной почты.');
    cy.get('input[type=email]').clear().type('abbos@mail.ru');

    cy.get('input[type=password]').type('asdsa');
    cy.contains('Пароль должен состоять минимум из 8 символов');
    cy.get('input[type=password]').type('Abbos1234$');
    cy.get('.help, .is-danger').should('not.exist');
  });

  it('Should visit to reset password module', () => {
    cy.get('a[href="/account/reset-password"]').click();
    cy.url().should('contain', '/reset-passwor');
  });
});
