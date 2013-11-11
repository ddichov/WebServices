$(document).ready(function (contacts) {
    // Get contacts container 
    var contactsHolder = $("#contacts");

    function renderContacts(contacts) {
        for (var contact in contacts) {
            var contactsHolder = $("<div class='row'id='" + contact.nickname
                          + "'><div class='two phone-one columns'>"
                          + "<a href='#'><img src='" + contact.imageUrl + "'></a> </div>"
                          + " <div class='ten phone-three columns'><h5 class='right'>0</h5>"
                          + "<h4><a href='#'>" + contact.nickname + "</a></h4> "
                          + "<h6 class='right'><a class='chatMate'id='" + "x-" + contact.nickname
                          + "' href='#'><span>Add to chat</span> +</a></h6>"
                          + "</div></div>");
        }

        contactsHolder.append(contactsHolder);
    };

  
  
});