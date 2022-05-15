# ICollections
ITransition internship task.

ICollections is a site for managing personal collections: books, stamps, badges, whiskey, etc.
Only read-only mode is available to unauthenticated users (search is available, creation of collections and items is not available, comments and likes are not available).

Authenticated users have access to everything except the admin panel.

The admin panel allows you to manage users (view, block, delete, appoint others as admins). The administrator sees each page of the user and each collection as its creator-owners (for example, he can edit or create a new collection on behalf of the user from his page or add an item, etc.). Only the owner or admin can manage the collection (edit/add/delete).

Login through registration on the site.

Full-text site search is available on each page (search results are always items, for example, if the text is found in the description of the collection or comments, which should be possible, then a link to the item is displayed).

Each user has his personal page, on which he manages the list of his collections (you can add, delete or edit) and from which you can go to the collection page (there is a table with filtering and sorting, the ability to create / delete / edit an item).

Each collection consists of: a name, a short description with support for markdown formatting, a "subject" (from a fixed set, for example, "Alcohol", "Books", etc.), an optional image (stored in the cloud, loaded by drag-n-drop- ohm). In addition, the collection has the ability to specify the fields that each item in it will have (there are fixed fields - id, title and tags, you can "add" some of the following - three numeric fields, three string fields, three text fields, three dates , three Boolean checkboxes). For example, you can specify that in my collection of books, each item has (in addition to id, title and tags) the string field "Author", the text field "Comment", the date field "Year of publication". A text field is a field with markdown formatting. The fields are characterized by a name. Fields are displayed in the list of items - the list needs support for switching sorts and filters.
Each item has tags (several tags are entered, auto-completion is required - when the user starts entering a tag, a list with word variants that have already been entered earlier on the site drops out).

The main page displays: recently added items, collections with the largest number of items, a tag cloud (when you click, the result is a list of links to items, similar to search results, in fact it can be one view).

When an item is opened in read mode by the author or simply by other users, comments are displayed at the end. Comments are linear, you cannot comment on comments, a new one is added only "in the tail". It is necessary to implement automatic loading of comments - if I have a page with comments open and someone else adds a new one, it automatically appears for me (a delay of 2-5 seconds is possible).

The item has likes (no more than one from one user per item).

The site supports two languages: Russian and English (the user chooses and the choice is saved). 
The site supports two designs (themes): light and dark.

https://user-images.githubusercontent.com/91548851/168489070-6fbbb3e3-7107-4ff4-8e76-317670f746a6.mp4

