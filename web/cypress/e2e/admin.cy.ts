describe('Admin page', () => {
  beforeEach(() => {
    cy.viewport(1366, 768);
  });

  it('Suggestion for changing password', () => {
    cy.visit('account/login');
    cy.get('input[type=email]').type('admin@mail.ru');
    cy.get('input[type=password]').type('Admin1234$');
    cy.get('button[type=submit]').click();

    cy.get('.notification').should('contain', 'Мы попросим вас сменить пароль для вашей безопасности');
    cy.get('.notification input[type=checkbox]').should('be.visible');

    cy.get('button[data-change-password]').should('contain.text', 'Изменить');
    cy.get('button[data-change-password]').click();

    const dialog = cy.get('div[data-srp-dialog]');
    dialog.should('exist');

    dialog.get('input[formcontrolname=currentPassword]').type('Admin1234$');
    dialog.get('input[formcontrolname=newPassword]').type('Administrator1234$');
    dialog.get('input[formcontrolname=confirmPassword]').type('Administrator1234$');
    dialog.get('button[type=submit]').click();

    cy.url().should('contain', 'login');
    cy.contains('Пароль изменён');
  });

  it('Teachers CRUD', () => {
    cy.visit('account/login');
    cy.get('input[type=email]').type('admin@mail.ru');
    cy.get('input[type=password]').type('Administrator1234$');
    cy.get('button[type=submit]').click();

    cy.url().should('contain', 'admin/teachers');

    cy.get('table.table').find('tr').should('have.length', 2);

    //check filter
    cy.get('input[data-srp-filter]').type('norkhujaev');
    cy.get('table.table').find('tr').should('have.length', 2);
    cy.get('input[data-srp-filter]').type('norkhujaev test');
    cy.get('table.table').should('contain', 'Данные не найдены.');
    cy.get('input[data-srp-filter]').clear();
    cy.get('table.table').find('tr').should('have.length', 2);

    cy.get('button[data-add-teacher]').click();

    cy.contains('Учетные данные');
    cy.get('input[type=email]').type('daler@mail.ru');
    cy.get('input[type=password]').type('Daler1234$');
    cy.get('button[data-dialog-submit]').click();

    cy.contains('Новый учитель');
    cy.get('#firstName').type('Daler');
    cy.get('#lastName').type('Nazarov');
    cy.get('#phoneNumber').type('901234567');
    cy.get('#dateOfBirth').clear().type('1950-10-15');
    cy.get('#dateOfBirth').clear().type('1950-10-15');
    cy.get('#address').type('Rudaki 123/3');
    cy.get('#country').type('Tajikistan');
    cy.get('#region').type('Badahshon');
    cy.get('#city').type('Xoruh');

    cy.get('button[data-dialog-submit]').click();
    cy.get('table.table').find('tr').should('have.length', 3);
    cy.get('tr[data-row-id="daler@mail.ru"]').dblclick();
  });
});
