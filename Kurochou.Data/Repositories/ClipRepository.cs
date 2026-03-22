using Kurochou.Domain.Entities;
using Kurochou.Domain.Interface.Repository;
using System.Data;

namespace Kurochou.Data.Repositories;

public class ClipRepository(IDbConnection conn) : Repository<Clip>(conn), IClipRepository
{
}