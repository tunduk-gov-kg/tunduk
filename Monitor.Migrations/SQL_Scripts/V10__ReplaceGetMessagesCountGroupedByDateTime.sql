drop function if exists "GetMessagesCountGroupedByDateTime"(bigint, bigint);

create or replace function "GetMessagesCountGroupedByDateTime"("from" timestamp without time zone,
                                                               "to" timestamp without time zone,
                                                               "groupBy" varchar,
                                                               "consumerInstance" varchar,
                                                               "consumerMemberClass" varchar,
                                                               "consumerMemberCode" varchar,
                                                               "consumerSubSystemCode" varchar,
                                                               "producerInstance" varchar,
                                                               "producerMemberClass" varchar,
                                                               "producerMemberCode" varchar,
                                                               "producerSubSystemCode" varchar,
                                                               "producerServiceCode" varchar,
                                                               "producerServiceVersion" varchar,
                                                               "succeeded" bool,
                                                               "includeMetaServices" bool)
    returns TABLE
            (
                "DateTime"      timestamp,
                "MessagesCount" bigint
            )
    language plpgsql
as
$$
begin
    return query select date_trunc("groupBy",
                                   COALESCE("ConsumerServerRequestIn", "ProducerServerRequestIn")) as "DateTime",
                        count(*)                                                                   as "MessagesCount"
                 from "Messages"

                 where COALESCE("ConsumerServerRequestIn", "ProducerServerRequestIn") >= "from"
                   and COALESCE("ConsumerServerRequestIn", "ProducerServerRequestIn") < "to"

                   and ("consumerInstance" isnull or "consumerInstance" = "ConsumerInstance")
                   and ("consumerMemberClass" isnull or "consumerMemberClass" = "ConsumerMemberClass")
                   and ("consumerMemberCode" isnull or "consumerMemberCode" = "ConsumerMemberCode")
                   and ("consumerSubSystemCode" isnull or "consumerSubSystemCode" = "ConsumerSubSystemCode")

                   and ("producerInstance" isnull or "producerInstance" = "ProducerInstance")
                   and ("producerMemberClass" isnull or "producerMemberClass" = "ProducerMemberClass")
                   and ("producerMemberCode" isnull or "producerMemberCode" = "ProducerMemberCode")
                   and ("producerSubSystemCode" isnull or "producerSubSystemCode" = "ProducerSubSystemCode")
                   and ("producerServiceCode" isnull or "producerServiceCode" = "ProducerServiceCode")
                   and ("producerServiceVersion" isnull or "producerServiceVersion" = "ProducerServiceVersion")
                   and ("succeeded" isnull or "succeeded" = "IsSucceeded")
                   and ("includeMetaServices" is null or CASE
                                                             when not "includeMetaServices"
                                                                 then ("ProducerServiceCode" not in (select * from "MetaServices"))
                                                             else true end)

                 group by date_trunc("groupBy",
                                     COALESCE("ConsumerServerRequestIn", "ProducerServerRequestIn"));

end;
$$;


