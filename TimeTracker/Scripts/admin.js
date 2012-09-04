$(document).ready(function () {
    $('#createUser').click(function () {
        $('.deleteEmployee').hide();
        $('.editEmployee').hide();
        $('.addEmployee').show();
    });

    $('#editUser').click(function () {
        $('.deleteEmployee').hide();
        $('.addEmployee').hide();
        $('.editEmployee').show();
    });

    $('#deleteUser').click(function () {
        $('.deleteEmployee').show();
        $('.addEmployee').hide();
        $('.editEmployee').hide();
    });

    $('#createTask').click(function () {
        $('.addEmployee').hide();
        $('.addTask').show();
    });

    $('#editU').click(function () {
        $('.editTask').hide();
        $('.editCard').hide();
        $('.editEmployee').show();
    });

    $('#editT').click(function () {
        $('.editEmployee').hide();
        $('.editCard').hide();
        $('.editTask').show();
    });

    $('#editC').click(function () {
        $('.editEmployee').hide();
        $('.editTask').hide();
        $('.editCard').show();
    });
});