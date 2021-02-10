﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sample.Core.MovieApplication.Notifications;
using Sample.DAL.Model.WriteModels;
using Sample.DAL.WriteRepositories;

namespace Sample.Core.MovieApplication.Commands
{
   public class AddMovieCommandHandler:IRequestHandler<AddMovieCommand,bool>
   {
       private readonly IMediator _mediator;
       private readonly WriteMovieRepository _movieRepository;

       public AddMovieCommandHandler(IMediator mediator, WriteMovieRepository movieRepository)
       {
           _mediator = mediator;
           _movieRepository = movieRepository;
       }

        public async Task<bool> Handle(AddMovieCommand request, CancellationToken cancellationToken)
        {
           _movieRepository.AddMovie(new Movie_Write
           {
               PublishYear = request.PublishYear,
               BoxOffice = request.BoxOffice,
               ImdbRate = request.ImdbRate,
               Name = request.Name
           });

           await _mediator.Publish(new AddReadModelNotification(request.Name, request.PublishYear, request.ImdbRate,
               request.BoxOffice), cancellationToken);

           return true;
        }
    }
}