from django.urls import path, include

urlpatterns = [
    path('search/', include('search.urls')),
]
