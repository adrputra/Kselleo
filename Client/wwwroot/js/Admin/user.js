$(document).ready(function () {
   var table = $('.tableUser').DataTable({
      filter: true,
      orderMulti: false,
      ajax: {
         url: 'https://localhost:5001/api/users',
         datatype: 'json',
         dataSrc: 'result',
      },
      columns: [
         {
            data: 'id',
         },
         {
            data: 'gender',
         },
         {
            data: 'fullName',
         },
         {
            data: 'email',
         },
         {
            className: 'dt-center',
            targets: '_all',
            orderable: false,
            data: null,
            render: function (data, type, row) {
               return `<button class="bi bi-trash-fill btn-secondary btn-sm" onclick='deleteuser("${row['id']}")'></button>
                            <button class='bi bi-pencil-square btn-secondary btn-sm' data-toggle="modal" onclick='GetDataUpdate("${row['id']}")' data-target='#updateUser'></button>`
            },
         },
      ],
   })
})

function deleteuser(id) {
   Swal.fire({
      title: 'Do you want to delete data?',
      showDenyButton: false,
      showCancelButton: true,
      confirmButtonText: 'Delete',
   }).then((result) => {
      /* Read more about isConfirmed, isDenied below */
      if (result.isConfirmed) {
         $.ajax({
            type: 'DELETE',
            url: 'https://localhost:5001/api/users/' + id,
         })
            .done((result) => {
               console.log(result)
            })
            .fail((error) => {
               console.log(error)
            })
         Swal.fire('Success Delete Data!', '', 'success')
      }
   })
}

function GetDataUpdate(id) {
   $.ajax({
      url: 'https://localhost:44308/api/users/' + id,
      success: function (results) {
         let result = results.result
         $('#IdUpdate').attr('value', `${result.id}`)
         $('#FullNameUpdate').attr('value', `${result.fullName}`)
         $('#EmailUpdate').attr('value', `${result.email}`)
         if (result.gender == 'Male') {
            $('#GenderUpdate').val('Male')
         } else $('#GenderUpdate').val('Female')
      },
   })
}

function update() {
   event.preventDefault()
   var obj1 = new Object()
   obj1.FullName = $('#FullNameUpdate').val()
   obj1.Email = $('#EmailUpdate').val()
   obj1.Gender = $('#GenderUpdate').val()
   console.log(obj1)

   $.ajax({
      url: 'https://localhost:44308/api/users/',
      type: 'PUT',
      headers: {
         Accept: 'application/json',
         'Content-Type': 'application/json',
      },
      dataType: 'json',
      data: JSON.stringify(obj1),
   })
      .done((result) => {
         console.log(result)
         Swal.fire({
            icon: 'success',
            title: 'SUCCESS',
         })
      })
      .fail((error) => {
         console.log(error)
         Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!',
         })
      })
}

function adduser() {
   event.preventDefault()
   var obj = new Object()
   obj.FullName = $('#fullName').val()
   obj.Email = $('#email').val()
   obj.Gender = $('#gender').val()
   obj.Password = $('#password').val()
   obj.ConfirmPassword = $('#confirmPassword').val()
   console.log(obj)

   $.ajax({
      url: 'https://localhost:44308/api/accounts/register',
      type: 'POST',
      headers: {
         Accept: 'application/json',
         'Content-Type': 'application/json',
      },
      dataType: 'json',
      data: JSON.stringify(obj),
   })
      .done((result) => {
         console.log(result)
         Swal.fire({
            icon: 'success',
            title: 'SUCCESS',
         })
      })
      .fail((error) => {
         console.log(error)
         Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Something went wrong!',
         })
      })
}
