const getBoardDetailById = (id, userId) => {
   $.ajax({
      type: 'GET',
      url: `https://localhost:5001/api/boards/detail/${id}`,
      dataType: 'json',
      success: function (response) {
         $('#title').html(response.data.name)
         $('#description').html(response.data.description)
         $('#createdAt').html(moment(response.data.createdAt).format('LLL'))

         let html = `<img src="https://ui-avatars.com/api/?name=${response.data.user.fullName}&background=random" alt="${response.data.user.fullName}" width="40px"
         class="rounded-circle" data-toggle="tooltip" data-placement="right" title="${response.data.user.fullName} - PM"
         style="margin-right: -20px;">`

         // append html to tag #members
         response.data.memberBoards
            .filter((member) => member.role != 'PM')
            .map((member) => {
               html += `<img src="https://ui-avatars.com/api/?name=${member.user.fullName}&background=random" alt="${member.role}" width="40px"
            class="rounded-circle" data-toggle="tooltip" data-placement="right" title="${member.user.fullName} - ${member.role}"
            style="margin-right: -20px;">`
            })

         $('#members').append(html)

         if (response.data.createdBy == userId) {
            $('#display-btn-invite').append(
               `
               <button class="btn btn-invite mr-4" style="background-color: #FAF5E4; color: #8B8B8B"
               data-bs-toggle="modal"
               data-bs-target="#inviteModal">+ Invite Member</button>
               `
            )
         }
      },
      error: function (e) {
         location.href = '/boards'
      },
   })
}

const inviteMember = (boardId) => {
   event.preventDefault()

   const req = {
      Email: $('#email').val(),
      Role: $('#role').val(),
      BoardId: boardId,
   }

   console.log(req)

   $.ajax({
      type: 'POST',
      url: 'https://localhost:5001/api/verifyinvites/verify',
      headers: {
         Accept: 'application/json',
         'Content-Type': 'application/json',
      },
      data: JSON.stringify(req),
      success: function (response) {
         $('#inviteModal').modal('hide')
         swal('Horee! Your invitation has been sent, waiting to be accepted!', {
            icon: 'success',
         })
      },
      error: function (e) {
         swal(`upps! ${JSON.parse(e.responseText).message}`, {
            icon: 'error',
         })
      },
   })
}
