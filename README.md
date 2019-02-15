# CGI-BusinessIdSpecification
This was a preliminary assignment for job interview.
## Task Description:
Create BusinessIdSpecification-class. The task of the class is to verify the correctness of the token given as a string, and to explain why the invalid business ID does not meet the requirements. The BusinessIdSpecification-class needed to implement the following interface:
```cs
public interface ISpecification<in TEntity>

{

    IEnumerable<string> ReasonsForDissatisfaction { get; }

    bool IsSatisfiedBy(TEntity entity);

}
```
