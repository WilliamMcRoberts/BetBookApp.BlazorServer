using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetBookData.Models;
using MediatR;

namespace BetBookData.Commands.UpdateCommands;

public record UpdateTeamCommand(TeamModel team) : IRequest<TeamModel>;

